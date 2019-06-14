using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildController_DotnetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(@"
            1. Interface
            2. ...Repository
            3. ...Resource
            4. ...Controller
            5. add line add MapperProfile + StartUp
    Enter <Capital word>: ");
            string cap = Console.ReadLine();
            //Console.Write("Enter <non-Cap>: ");
            //string noncap = Console.ReadLine();
            string resultInterface = generatedInterface(cap);
            Console.WriteLine(resultInterface);
            Console.ReadLine();

            string resultRepository = generatedRepository(cap);
            Console.WriteLine(resultRepository);
            Console.ReadLine();

            string resultController = generatedController(cap,cap.ToLower());
            Console.WriteLine(resultController);
            Console.ReadLine();

            string resultMap = generatedMappingProfileAndStartUp(cap);
            Console.WriteLine(resultMap);
            Console.ReadLine();

            //string resultStartUp = generatedStartup(cap);
            //Console.WriteLine(resultStartUp);
            //Console.ReadLine();
        }
        public static string generatedInterface(string cap)
        {
            return string.Format(@"
            ------------------------------
            Interface
            ------------------------------
            
public interface I{0}Repository : IRepository<{0}>", cap);
        }

        public static string generatedRepository(string cap)
        {
            return string.Format(@"

            ------------------------------
            Repository name: {0}Repository
            ------------------------------
            
using HR_API.Models;

namespace HR_API.DataAccess.Repositories
<*
public class {0}Repository : BaseRepository<{0}>, I{0}Repository
    <*
        private readonly HRDataContext _context;
        public {0}Repository(HRDataContext context) : base(context)
        <*
            _context = context;
        >*
    >*
>*", cap);
    }

        public static string generatedController(string cap, string noncap)
        {
            return string.Format(@"
            ------------------------------
            Controller name: {0}Controller
            Resource name: {0}Resource
            ------------------------------
            
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR_API.Controllers.Resources;
using HR_API.DataAccess.Repositories;
using HR_API.DataAccess.UnitOfWork;
using HR_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR_API.Controllers
<*
    [Route(""api/[controller]"")]
    [ApiController]
    public class {0}Controller : ControllerBase
        <*
            private readonly I{0}Repository _{1}Repo;
            private readonly IUnitOfWork _uom;
            private readonly IMapper _mapper;

            public {0}Controller(I{0}Repository {1}Repo, IUnitOfWork uom, IMapper mapper) : base()
            <*
                _mapper = mapper;
                _{1}Repo = {1}Repo;
                _uom = uom;
            >*

            [HttpGet(""all"")]
            public async Task<ActionResult> GetAll{0}()
            <*
                var {1} = await _{1}Repo.GetAllAsync();
                var {1}List = _mapper.Map<List<{0}Resource>>({1});
                return Ok({1}List);
            >*

            [HttpGet(""<*id>*"")]
            public async Task<ActionResult> Get{0}ById(int id)
            <*
                var {1} = await _{1}Repo.GetByIdAsync(id);
                if ({1} != null)
                <*
                    var {1}Return = _mapper.Map<{0}Resource>({1});
                    return Ok({1}Return);
                >*
                return Unauthorized();
            >*

            [HttpPost]
            public async Task<IActionResult> Add{0}([FromBody] {0} {1})
            <*
                //Check model validity
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _{1}Repo.AddAsync({1});
                await _uom.CommitAsync();
                return Created(""{0} was created."", _mapper.Map<{0}Resource>({1}));
            >*

            // PUT api/values/5
            [HttpPut(""<*id>*"")]
            public async Task<ActionResult> Update{0}([FromBody] {0} {1})
            <*
                _{1}Repo.Update({1});
                await _uom.CommitAsync();
                return Ok(_mapper.Map<{0}Resource>({1}));
            >*

            // DELETE api/values/5
            [HttpDelete(""<*id>*"")]
            public async Task<ActionResult> Delete{0}(int id)
            <*
                var {1} = await _{1}Repo.GetByIdAsync(id);
                if ({1} != null)
                <*
                    _{1}Repo.Delete({1}.{0}ID);
                    await _uom.CommitAsync();
                    return Ok();
                >*
                return Unauthorized();
            >*
        >*
    >*", cap, noncap);
        }

        public static string generatedMappingProfileAndStartUp(string cap)
        {
            return string.Format(@"
            ------------------------------
            MappingProfile
            ------------------------------
CreateMap<{0}, {0}Resource>().ReverseMap();
            ------------------------------
            StartUp
            ------------------------------
services.AddScoped<I{0}Repository, {0}Repository>();", cap);
        }

    }
}
