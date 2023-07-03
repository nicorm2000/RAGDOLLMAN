using UnityEngine;

/// <summary>
/// A singleton class that allows teleporting items to a specified destination.
/// </summary>
public class ItemTeleporter
{
    private static ItemTeleporter instance;
    public static ItemTeleporter Instance => instance ??= new ItemTeleporter();

    private ItemTeleporter() { }

    /// <summary>
    /// Teleports the specified item to the destination.
    /// </summary>
    /// <param name="item">The item to teleport.</param>
    /// <param name="destination">The destination transform to teleport the item to.</param>
    public void TeleportItem(GameObject item, Transform destination)
    {
        item.transform.position = destination.position;
    }
}