namespace LobbyLib;

/// <summary>
/// Provides a message from result code.
/// </summary>
public static class ResultCodeMessages
{
    /// <summary>
    /// Get a message from <paramref name="code"/>.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <returns>The message.</returns>
    public static string GetMessage(this ResultCode code)
    {
        return code switch
        {
            ResultCode.Unknown => "Unknown Error!",
            ResultCode.Success => string.Empty,
            ResultCode.NoAccountFound => "No Account found with the id provided!",
            ResultCode.AlreadyFriend => "Action cannot be done since user already your friend!",
            ResultCode.NotFoundInRequest => "Action cannot be done since friend request not found in your account!",
            ResultCode.IsInBlockList => "Action cannot be done since id is in block list!",
            ResultCode.NotFoundInFriends => "Action cannot be done since user not your friend!",
            _ => "Unknown Error!",
        };
    }
}
