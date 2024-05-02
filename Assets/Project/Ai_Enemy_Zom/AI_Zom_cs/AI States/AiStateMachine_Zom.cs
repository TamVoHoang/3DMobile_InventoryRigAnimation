using UnityEngine;

public class AiStateMachine_zom
{
    public AiAgent_zom agent_zom;
    public AiState_Zom[] states;
    public AiStateID_Zom currentState;
    public int numStates_AiStateID_zom;

    public AiStateMachine_zom(AiAgent_zom aiAgent_Zom) {
        this.agent_zom = aiAgent_Zom;
        int numStates = System.Enum.GetValues(typeof(AiStateID_Zom)).Length; 
        numStates_AiStateID_zom = numStates;
        states = new AiState_Zom[numStates];
    }

    public void RegisterState(AiState_Zom state) {
        int index = (int)state.GetId();
        Debug.Log("checking su kien dang ky tra ve int = " + state.GetId().ToString());
        states[index] = state;

        /* int[] a = new a[3]
        int a[0] = 1 */
    }
    public AiState_Zom GetState(AiStateID_Zom stateID) {
        int index = (int)stateID;
        return states[index];
    }

    public void Update() {
        GetState(currentState)?.Update(agent_zom);
    }
    public void ChangeState(AiStateID_Zom newState) {
        GetState(newState)?.Exit(agent_zom);
        currentState = newState;
        GetState(currentState)?.Enter(agent_zom);
        /* if(GetState(currentState) != null) {
            GetState(currentState).Enter(agent);
        } */
    }
}
