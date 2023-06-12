using UnityEngine;

public class PlayerTeleporter
{
    private static PlayerTeleporter instance;
    public static PlayerTeleporter Instance => instance ??= new PlayerTeleporter();

    private PlayerTeleporter() { }

    /// <summary>
    /// Teleports all child objects of the player to the destination.
    /// </summary>
    /// <param name="player">The player game object whose children will be teleported.</param>
    /// <param name="destination">The destination transform to teleport the children to.</param>
    public void TeleportChildren(GameObject player, Transform destination)
    {
        for (int i = 0; i < player.transform.parent.childCount; i++)
        {
            GameObject child = player.transform.parent.GetChild(i).gameObject;
            ItemTeleporter.Instance.TeleportItem(child, destination);
        }
    }
}