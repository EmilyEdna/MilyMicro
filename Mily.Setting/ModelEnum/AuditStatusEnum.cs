using System.ComponentModel;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 审核枚举
    /// </summary>
    public enum AuditStatusEnum
    {
        /// <summary>
        /// 审核不通过
        /// </summary>
        [Description("审核不通过")]
        AccessFail,

        /// <summary>
        /// 等待审核
        /// </summary>
        [Description("等待审核")]
        WaitAudti,

        /// <summary>
        /// 待上级审核
        /// </summary>
        [Description("待上级审核")]
        WaitSuperiorAudit,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        AccessSuccess,

        /// <summary>
        /// 等待终审
        /// </summary>
        [Description("等待终审")]
        WaitFinal,

        /// <summary>
        /// 终审不通过
        /// </summary>
        [Description("终审不通过")]
        AccessFinalFail,

        /// <summary>
        /// 终审通过
        /// </summary>
        [Description("终审通过")]
        AccessFinalSuccess
    }
}