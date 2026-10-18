using EIV_LobbyData.Socket.CustomMessages;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.CustomMessageProcessor;

/// <summary>
/// Process <see cref="MatchmakeCheck"/>.
/// </summary>
public class MatchmakeCheckProcessor : BaseProcessor<MatchmakeCheck>
{
    /// <inheritdoc/>
    protected override bool OnProcess([NotNull] MatchmakeCheck message)
    {
        return SendResponse(ResultCode.Success);
    }
}
