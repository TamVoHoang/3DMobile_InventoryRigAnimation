using UnityEngine;

public class AiStateMachine
{
    // todo ket noi AiAgent va AiStateMachine
    // todo duoc khoi tao tu AiAgent -> AiObject
    public AiAgent agent;                     // doi tuong can change behavour
    public AiState[] states;                  // mang phan tu kieu aiState enum
    public AiStateID currentState;
    public int numStates_AiStateID;

    public AiStateMachine(AiAgent aiAgent) {
        Debug.Log("AiAgent khoi tao + gan new AiStateMachine(this)");

        this.agent = aiAgent;
        int numStates = System.Enum.GetValues(typeof(AiStateID)).Length;    //so states trong AiStateID enum
        numStates_AiStateID = numStates;
        states = new AiState[numStates]; //int[] a -->> a = new int[3]

    }

    public void RegisterState(AiState state) {
        int index = (int)state.GetId();
        Debug.Log("checking su kien dang ky tra ve int = " + state.GetId().ToString());
        states[index] = state;

        /* int[] a = new a[3]
        int a[0] = 1 */
    }

    public AiState GetState(AiStateID stateID) {
        int index = (int)stateID;
        return states[index];
    }

    
    public void Update() {
        GetState(currentState)?.Update(agent);
    }
    public void ChangeState(AiStateID newState) {
        GetState(newState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
        /* if(GetState(currentState) != null) {
            GetState(currentState).Enter(agent);
        } */
    }


}
