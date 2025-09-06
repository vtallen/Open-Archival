using Microsoft.EntityFrameworkCore;
using OpenArchival.DataAccess;

namespace OpenArchival.Blazor;

public class ArtifactEntrySharedHelpers
{
    IArtifactDefectProvider DefectsProvider { get; set; }

    IArtifactStorageLocationProvider StorageLocationProvider { get; set; }

    IArchiveEntryTagProvider TagsProvider { get; set; } 

    IArtifactTypeProvider TypesProvider { get; set; }

    IListedNameProvider ListedNameProvider { get; set; }
    
    IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

    IArtifactGroupingProvider GroupingProvider { get; set; }


    public ArtifactEntrySharedHelpers(IArtifactDefectProvider defectsProvider, IArtifactStorageLocationProvider storageLocationProvider, IArchiveEntryTagProvider tagsProvider, IArtifactTypeProvider typesProvider, IListedNameProvider listedNamesProvider, IDbContextFactory<ApplicationDbContext> contextFactory, IArtifactGroupingProvider groupingProvider)
    {
        DefectsProvider = defectsProvider;
        StorageLocationProvider = storageLocationProvider;
        TagsProvider = tagsProvider;
        TypesProvider = typesProvider;
        ListedNameProvider = listedNamesProvider;
        DbContextFactory = contextFactory;
        GroupingProvider = groupingProvider;
    }

    public async Task<IEnumerable<string>> SearchDefects(string value, CancellationToken cancellationToken)
    {
        List<string> defects;
        if (string.IsNullOrEmpty(value))
        {
            defects = new((await DefectsProvider.Top(25) ?? []).Select(prop => prop.Description));
        }
        else
        {
            defects = new((await DefectsProvider.Search(value) ?? []).Select(prop => prop.Description));
        }

        return defects;
    }

    public async Task<IEnumerable<string>> SearchStorageLocation(string value, CancellationToken cancellationToken)
    {
        List<string> storageLocations;
        if (string.IsNullOrEmpty(value))
        {
            storageLocations = new((await StorageLocationProvider.Top(25) ?? []).Select(prop => prop.Location));
        }
        else
        {
            storageLocations = new((await StorageLocationProvider.Search(value) ?? []).Select(prop => prop.Location));
        }

        return storageLocations;
    }

    public async Task<IEnumerable<string>> SearchTags(string value, CancellationToken cancellationToken)
    {
        List<string> tags;
        if (string.IsNullOrEmpty(value))
        {
            tags = new((await TagsProvider.Top(25) ?? []).Select(prop => prop.Name));
        }
        else
        {
            tags = new((await TagsProvider.Search(value) ?? []).Select(prop => prop.Name));
        }

        return tags;
    }

    public async Task<IEnumerable<string>> SearchItemTypes(string value, CancellationToken cancellationToken)
    {
        List<string> itemTypes;
        if (string.IsNullOrEmpty(value))
        {
            itemTypes = new((await TypesProvider.Top(25) ?? []).Select(prop => prop.Name));
        }
        else
        {
            itemTypes = new((await TypesProvider.Search(value) ?? []).Select(prop => prop.Name));
        }

        return itemTypes;
    }

    public async Task<IEnumerable<string>> SearchListedNames(string value, CancellationToken cancellationToken)
    {
        List<ListedName> names;
        if (string.IsNullOrEmpty(value))
        {
            names = new((await ListedNameProvider.Top(25) ?? []));
        }
        else
        {
            names = new((await ListedNameProvider.Search(value) ?? []));
        }

        return names.Select(p => p.Value);
    }

    /*
    public async Task OnGroupingPublished(ArtifactGroupingValidationModel model)
    {
        await using var context = await DbContextFactory.CreateDbContextAsync();
        
        






        var grouping = model.ToArtifactGrouping();

        // The old logic for attaching the category is still good.
        context.Attach(grouping.Category);

        // 1. Handle ArtifactType (no change, this was fine)
        if (grouping.Type is not null)
        {
            var existingType = await context.ArtifactTypes
                .FirstOrDefaultAsync(t => t.Name == grouping.Type.Name);

            if (existingType is not null)
            {
                grouping.Type = existingType;
            }
        }

        // 2. Process ChildArtifactEntries
        foreach (var entry in grouping.ChildArtifactEntries)
        {
            // Handle ArtifactStorageLocation (no change, this was fine)
            var existingLocation = await context.ArtifactStorageLocations
                .FirstOrDefaultAsync(l => l.Location == entry.StorageLocation.Location);

            if (existingLocation is not null)
            {
                entry.StorageLocation = existingLocation;
            }

            // Handle Defects
            if (entry.Defects is not null && entry.Defects.Any())
            {
                var defectDescriptions = entry.Defects.Select(d => d.Description).ToList();
                var existingDefects = await context.ArtifactDefects
                    .Where(d => defectDescriptions.Contains(d.Description))
                    .ToListAsync();

                // Replace in-memory defects with existing ones
                for (int i = 0; i < entry.Defects.Count; i++)
                {
                    var existingDefect = existingDefects
                        .FirstOrDefault(ed => ed.Description == entry.Defects[i].Description);

                    if (existingDefect is not null)
                    {
                        entry.Defects[i] = existingDefect;
                    }
                }
            }

            // Handle ListedNames
            if (entry.ListedNames is not null && entry.ListedNames.Any())
            {
                var listedNamesValues = entry.ListedNames.Select(n => n.Value).ToList();
                var existingNames = await context.ArtifactAssociatedNames
                    .Where(n => listedNamesValues.Contains(n.Value))
                    .ToListAsync();

                for (int i = 0; i < entry.ListedNames.Count; i++)
                {
                    var existingName = existingNames
                        .FirstOrDefault(en => en.Value == entry.ListedNames[i].Value);

                    if (existingName is not null)
                    {
                        entry.ListedNames[i] = existingName;
                    }
                }
            }

            // Handle Tags
            if (entry.Tags is not null && entry.Tags.Any())
            {
                var tagNames = entry.Tags.Select(t => t.Name).ToList();
                var existingTags = await context.ArtifactEntryTags
                    .Where(t => tagNames.Contains(t.Name))
                    .ToListAsync();

                for (int i = 0; i < entry.Tags.Count; i++)
                {
                    var existingTag = existingTags
                        .FirstOrDefault(et => et.Name == entry.Tags[i].Name);

                    if (existingTag is not null)
                    {
                        entry.Tags[i] = existingTag;
                    }
                }
            }

            // 💡 NEW: Handle pre-existing FilePathListings
            // This is the key change to resolve the exception
            if (entry.Files is not null)
            {
                foreach (var filepath in entry.Files)
                {
                    // The issue is trying to add a new entity that has an existing primary key.
                    // Since you stated that all files are pre-added, you must attach them.
                    // Attach() tells EF Core to track the entity, assuming it already exists.
                    context.Attach(filepath);
                    // Also ensure the parent-child relationship is set correctly, though it's likely set by ToArtifactGrouping
                    filepath.ParentArtifactEntry = entry;
                }
            }
            // Tag each entry with the parent grouping so it is linked correctly in the database
            entry.ArtifactGrouping = grouping;
        }

        // 3. Add the main grouping object and let EF Core handle the graph
        // The previous issues with the graph are resolved, so this line should now work.
        context.ArtifactGroupings.Add(grouping);

        // 4. Save all changes in a single transaction
        await context.SaveChangesAsync();
    }
    */

    public async Task OnGroupingPublished(ArtifactGroupingValidationModel model)
    {
        // The OnGroupingPublished method in this class should not contain DbContext logic.
        // It should orchestrate the data flow by calling the appropriate provider methods.
        var isNew = model.Id == 0 || model.Id is null;

        // Convert the validation model to an entity
        var grouping = model.ToArtifactGrouping();

        if (isNew)
        {
            // For a new grouping, use the CreateGroupingAsync method.
            // The provider method will handle the file path logic.
            await GroupingProvider.CreateGroupingAsync(grouping);
        }
        else
        {
            // For an existing grouping, use the UpdateGroupingAsync method.
            // The provider method will handle the change tracking.
            await GroupingProvider.UpdateGroupingAsync(grouping);
        }
    }
}
