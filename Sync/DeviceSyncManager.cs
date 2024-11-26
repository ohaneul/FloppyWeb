public class DeviceSyncManager
{
    private readonly CloudStorage cloudStorage;
    private readonly EncryptionService encryption;
    private readonly ConflictResolver conflictResolver;

    // Because losing your bookmarks sucks
    public async Task SyncUserData()
    {
        var userData = await CollectUserData();
        var encrypted = await encryption.EncryptData(userData);
        
        try
        {
            await cloudStorage.UploadAsync(encrypted);
        }
        catch (Exception ex)
        {
            // Store locally until we can sync
            await StoreForLaterSync(encrypted);
            NotifyUser("We'll sync your data when you're back online! 📡");
        }
    }

    // Added after that one time everything got duplicated
    private async Task<MergeResult> HandleConflicts(UserData local, UserData remote)
    {
        var conflicts = conflictResolver.FindConflicts(local, remote);
        if (conflicts.Any())
        {
            return await ShowConflictResolutionDialog(conflicts);
        }
        return await conflictResolver.AutoResolve(local, remote);
    }
} 