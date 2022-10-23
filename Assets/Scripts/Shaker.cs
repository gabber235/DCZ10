using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Have the shaker switch between the list of players. Such that only one player has the shaker.
/// </summary>
public class Shaker : MonoBehaviour
{
    public List<Player> players;
    public List<InputActionReference> throwsActionReference;
    public int currentShakerIndex;
    public Vector3 offset;
    public Vector3 LOS_offset;

    private Vector3 _velocity;

    private void Start()
    {
        for (var i = 0; i < throwsActionReference.Count; i++)
        {
            var index = i;
            throwsActionReference[i].action.Enable();
            throwsActionReference[i].action.performed += _ => SwitchShaker(index);
        }

        players[currentShakerIndex].shaker = true;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            players[currentShakerIndex].transform.position + offset, 
            ref _velocity, 
            0.1f
            );
        
        Debug.DrawRay(players[0].transform.position + LOS_offset, (players[1].transform.position + LOS_offset) - (players[0].transform.position + LOS_offset), Color.yellow);
    }

    // If the player is holding the shaker, switch to the next player
    private void SwitchShaker(int index)
    {
        if (currentShakerIndex != index) return;
        
        // Ensure line-of-sight when switching shaker
        RaycastHit hit;
        if (Physics.SphereCast(players[0].transform.position, 0.5f, players[1].transform.position - players[0].transform.position, out hit)) {
            if (hit.collider.CompareTag("Player")) {
                Debug.Log("Player in sight");
                players[currentShakerIndex].shaker = false;
                currentShakerIndex = (currentShakerIndex + 1) % players.Count;
                players[currentShakerIndex].shaker = true;
            }
            else {
                Debug.Log("No Player LOS");
            }
        }

        
    }
}