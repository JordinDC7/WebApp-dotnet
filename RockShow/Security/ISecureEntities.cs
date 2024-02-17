using RockShow.Enums;

namespace RockShow.Security
{
    public interface ISecureEntities<T, K>
    {
        bool IsAuthorized(Task userId, K entityId, EntityActionType actionType, EntityType entityType);
    }
}
