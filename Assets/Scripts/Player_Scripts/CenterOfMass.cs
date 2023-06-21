using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    /// <summary>
    /// Calculates the center of mass of a rigid body based on the positions of the colliders weighted by their mass, then sets the position of the COMP transform to the resulting center of mass.
    /// </summary>
    /// <param name="COMP">The transform to set to the final center of mass position.</param>
    /// <param name="centerOfMassPoint">The center of mass point for the entire rigid body.</param>
    /// <param name="playerParts">An array of 13 GameObjects representing the 13 parts of the rigid body.</param>
    public void CenterOfMassCalculation(Transform COMP, Vector3 centerOfMassPoint, GameObject[] playerParts)
    {
        //Character's center of mass.
        //The center of mass can be calculated by taking the masses you are trying to find the center of mass between and multiplying them by their positions.
        //Then, you add these together and divide that by the sum of all the individual masses.
        //Formula -> centerOfMassPoint = (m1* p1 + m2* p2 + ... + mn* pn) / (m1 + m2 + ... + mn)
        //m is the mass of each collider, and p is the position of each collider
        //The result is the center of mass point of the entire rigid body.

        Vector3 dividendSum = Vector3.zero;

        float divisorSum = 0f;

        for (int i = 0; i < 13; i++)
        {
            dividendSum += CenterOfMassPointDividend(i, centerOfMassPoint, playerParts);
            divisorSum += CenterOfMassPointDivisor(i, playerParts);
        }

        Vector3 centerOfMass = dividendSum / divisorSum;

        COMP.position = centerOfMass;
    }

    /// <summary>
    /// Calculates the dividend of the center of mass calculation for a single rigid body part by multiplying the part's mass by its position, then returns the resulting vector.
    /// </summary>
    /// <param name="arrayPosition">The position of the rigid body part in the playerParts array.</param>
    /// <param name="centerOfMassPoint">The center of mass point for the entire rigid body.</param>
    /// <param name="playerParts">An array of 13 GameObjects representing the 13 parts of the rigid body.</param>
    /// <returns>A vector representing the dividend calculation for a single rigid body part.</returns>
    private Vector3 CenterOfMassPointDividend(int arrayPosition, Vector3 centerOfMassPoint, GameObject[] playerParts)
    {
        centerOfMassPoint = playerParts[arrayPosition].GetComponent<Rigidbody>().mass * playerParts[arrayPosition].transform.position;

        return centerOfMassPoint;
    }

    /// <summary>
    /// Calculates the divisor of the center of mass calculation for a single rigid body part by returning the part's mass.
    /// </summary>
    /// <param name="arrayPosition">The position of the rigid body part in the playerParts array.</param>
    /// <param name="playerParts">An array of 13 GameObjects representing the 13 parts of the rigid body.</param>
    /// <returns>The mass of a single rigid body part.</returns>
    private float CenterOfMassPointDivisor(int arrayPosition, GameObject[] playerParts)
    {
        float centerOfMassPoint = playerParts[arrayPosition].GetComponent<Rigidbody>().mass;

        return centerOfMassPoint;
    }
}