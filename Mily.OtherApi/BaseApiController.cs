using Microsoft.AspNetCore.Mvc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;

namespace Mily.OtherApi
{
    public class BaseApiController : ControllerBase, IClientService
    {
    }
}