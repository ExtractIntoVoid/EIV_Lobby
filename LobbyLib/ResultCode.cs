namespace LobbyLib;

/// <summary>
/// Represent a result code.
/// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32
public enum ResultCode : uint
#pragma warning restore CA1028 // Enum Storage should be Int32
{
    /// <summary>
    /// Unknown result.
    /// </summary>
    Unknown = uint.MaxValue,

    /// <summary>
    /// Result run successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// No Account found with the id provided.
    /// </summary>
    NoAccountFound,

    /// <summary>
    /// Action cannot be done since user already your friend.
    /// </summary>
    AlreadyFriend,

    /// <summary>
    /// Action cannot be done since friend request not found in your account.
    /// </summary>
    NotFoundInRequest,

    /// <summary>
    /// Action cannot be done since id is in block list.
    /// </summary>
    IsInBlockList,

    /// <summary>
    /// Action cannot be done since user not your friend.
    /// </summary>
    NotFoundInFriends,
}