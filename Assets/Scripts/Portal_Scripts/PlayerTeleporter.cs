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
        Transform root = GetRoot(player.transform);

        ItemTeleporter.Instance.TeleportItem(root.gameObject, destination);
    }

    /// <summary>
    /// Gets Root transform from the player
    /// </summary>
    /// <param name="player">Everty part of the body that will be checked to look for the Root.</param>
    private Transform GetRoot(Transform player)
    {
        if (player.parent.tag == "Player") 
        {
            return player.parent;
        }
        else
        {
            //Recursive function to check for the root transform if it is not found
            return GetRoot(player.transform.parent);
        }
    }
}