using Rockets.Api.Requests;
using Lunar.Rockets.GettingCartAtVersion;
using Lunar.Rockets.GettingCartById;
using Lunar.Rockets.GettingCartHistory;
using Lunar.Rockets.GettingCarts;
using Microsoft.AspNetCore.Mvc;
using Core.Commands;
using Core.Ids;
using Core.Marten.Responses;
using Core.Queries;
using Core.Responses;
using Core.WebApi.Headers;
using Marten.Pagination;
using System.Threading.Tasks;
using System;
using Lunar.Rockets.Launching;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.ChangingMission;
using Lunar.Rockets.ExploadingRocket;

namespace Rockets.Api.Controllers;

[Route("api/[controller]")]
public class RocketsController: Controller
{
    private readonly ICommandBus commandBus;
    private readonly IQueryBus queryBus;
    private readonly IIdGenerator idGenerator;

    public RocketsController(
        ICommandBus commandBus,
        IQueryBus queryBus,
        IIdGenerator idGenerator)
    {
        this.commandBus = commandBus;
        this.queryBus = queryBus;
        this.idGenerator = idGenerator;
    }

    [HttpPost]
    public async Task<IActionResult> ConsumeRocketEvent([FromBody] RocketEvent? request)
    {

        if (request == null)
            return BadRequest("request cannot be null");

        if (request.MetaData == null)
            return BadRequest("request must have valid meta data");

        if (String.IsNullOrEmpty(request.MetaData.MessageType))
            return BadRequest("request must have valid messageType");


        //TODO: Add more validation

        dynamic policy = null;


        switch (request.MetaData.MessageType)
        {
            case "RocketLaunched":
                {


                    var timestamp = request.MetaData.MessageTime.DateTime;
                    string type = request.Message.type;
                    string mission = request.Message.mission;
                    long speed = request.Message.launchSpeed;

                    policy = LaunchRocket.Create(
                        request.MetaData.Channel,
                        request.MetaData.MessageNumber,
                        timestamp,
                        type,
                        mission,
                        speed
                        );
                    break;
                }
            case "RocketSpeedIncreased":
                {
                    long speedIncrement = request.Message.by;

                    policy = IncreaseRocketSpeed.Create(
                        request.MetaData.Channel,
                        speedIncrement
                        );
                    break;
                }
            case "RocketSpeedDecreased":
                {
                    long speedDecrement = request.Message.by;

                    policy = IncreaseRocketSpeed.Create(
                        request.MetaData.Channel,
                        speedDecrement
                        );
                    break;
                }
            case "RocketMissionChanged":
                {
                    string newMission = request.Message.newMission;

                    policy = ChangeMission.Create(
                        request.MetaData.Channel,
                        newMission
                        );
                    break;
                }

            case "RocketExploaded":
                {
                    string reason = request.Message.reason;
                    var timestamp = request.MetaData.MessageTime.DateTime;

                    policy = ExploadRocket.Create(
                        request.MetaData.Channel,
                        reason,
                        timestamp
                        );
                    break;
                }

            default:
                {
                    return NotFound($"message type {request.MetaData.MessageType} is not supported");
                }
        }

      

        await commandBus.Send(policy);

        return Accepted("api/Rockets", request.MetaData.Channel);
    }



    [HttpGet("{id}")]
    public async Task<RocketDetails> Get(Guid id)
    {
        var result = await queryBus.Send<GetRocketById, RocketDetails>(GetRocketById.Create(id));

        Response.TrySetETagResponseHeader(result.Version.ToString());

        return result;
    }

    [HttpGet]
    public async Task<PagedListResponse<RocketInfo>> Get([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var pagedList = await queryBus.Send<GetRockets, IPagedList<RocketInfo>>(GetRockets.Create(pageNumber, pageSize));

        return pagedList.ToResponse();
    }


    [HttpGet("{id}/history")]
    public async Task<PagedListResponse<RocketHistory>> GetHistory(Guid id)
    {
        var pagedList = await queryBus.Send<GetRocketHistory, IPagedList<RocketHistory>>(GetRocketHistory.Create(id));

        return pagedList.ToResponse();
    }

    [HttpGet("{id}/versions")]
    public Task<RocketDetails> GetVersion(Guid id, [FromQuery] GetRocketAtVersion? query)
    {
        return queryBus.Send<GetRocketAtVersion, RocketDetails>(GetRocketAtVersion.Create(id, query?.Version));
    }
}
