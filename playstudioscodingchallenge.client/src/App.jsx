import { useEffect, useState } from 'react';
import * as React from "react";
import './App.css';

function App() {
    const [forecasts, setForecasts] = useState();
    const [questProgress, setQuestProgress] = useState();
    const [questState, setQuestState] = useState();
    const [playerId, setPlayerId] = useState("");
    const [playerLevel, setPlayerLevel] = useState(1);
    const [chipAmountBet, setChipAmountBet] = useState(1);

    const handlePlayerIdChange = (event) => {
      setPlayerId(event.target.value);
    };

    const handlePlayerLevelChange = (event) => {
      setPlayerLevel(event.target.value);
    };

    const handleChipAmountBetChange = (event) => {
      setChipAmountBet(event.target.value);
    };

    const style = {
      display: "flex",
      flexDirection: "row"
    };

    const progress = async () => {
      try {
        var inputData = new Object();
        inputData.playerId = playerId;
        inputData.playerLevel = playerLevel;
        inputData.chipAmountBet = chipAmountBet;

        const response = await fetch('api/progress', {
          method: "POST",
          body: JSON.stringify(inputData),
          headers: {
          'Content-Type': 'application/json'
          }
        });

        if (response.ok) {
          response.json().then((data) => {
            setQuestProgress(data);
          });
        }
        else {
          window.alert("Error: " + response.status + " " + response.statusText);
        }
      }
      catch (e) {
        window.alert(e);
      }
    }

    const state = async () => {
      try {
        const response = await fetch(`api/state?playerId=${playerId}`, {
          method: "GET",
          headers: {
          'Content-Type': 'application/json'
          }
        });

        if (response.ok) {
          response.json().then((data) => {
            setQuestState(data);
          });
        }
        else {
          window.alert("Error: " + response.status + " " + response.statusText);
        }
      }
      catch (e) {
        window.alert(e);
      }
    }

    const getPlayerId = async () => {
       try {
        const response = await fetch(`api/getplayerid`, {
          method: "GET",
          headers: {
          'Content-Type': 'application/json'
          }
        });

        if (response.ok) {
          response.json().then((data) => {
            setPlayerId(data);
          });
        }
        else {
          window.alert("Error: " + response.status + " " + response.statusText);
        }
      }
      catch (e) {
        window.alert(e);
      }
    }

  const stateResult = questState === undefined
    ? <></>
    : <div>  
      <p><b>Total Quest Percent Completed: </b>{questState.totalQuestPercentCompleted}%</p>
      <p><b>Last Milestone Index Completed: </b>{questState.lastMilestoneIndexCompleted}</p>
    </div>;

    const progressResult = questProgress === undefined
    ? <></>
    : <div>
      <p><b>Quest Points Earned: </b>{questProgress.questPointsEarned}</p>
      <p><b>Total Quest Percent Completed: </b> {questProgress.totalQuestPercentCompleted}%</p>
      <p><b>Milestones Completed: </b></p>
      <table>
            <thead>
                <tr>
                    <th>Milestone Index</th>
                    <th>Chips Awarded</th>
                </tr>
            </thead>
            <tbody>
                {questProgress.milestonesCompleted.length !== 0 && questProgress.milestonesCompleted.map(milestone =>
                    <tr key={milestone.milestoneIndex}>
                        <td>{milestone.milestoneIndex}</td>
                        <td>{milestone.chipsAwarded}</td>
                    </tr>
                )}
            </tbody>
    </table>
    </div>;

    return (
        <div>
            <h1 id="tableLabel">Quest API</h1>
            <button onClick={getPlayerId}>Get Player ID</button>
            <br/><br/>
            <div style={{display: "flex", flexDirection: "row", gap:"10px"}}>
              <div style={{padding: "15px", border: "2px solid grey", borderRadius: "6px"}}>
                <h2>api/progress</h2>
                Player Id: <input type="text" name="playerId" size="50" value={playerId} onChange={handlePlayerIdChange} />
                <br/><br/>
                Player Level: <input type="number" name="playerLevel" value={playerLevel} onChange={handlePlayerLevelChange} />
                <br/><br/>
                Chip Amount Bet: <input type="number" name="chipAmountBet" value={chipAmountBet} onChange={handleChipAmountBetChange} />
                <br/><br/>
                {/*<input type="submit" value="Submit" />*/}
                <button onClick={progress}>Progress Quest</button>
                <br/>
                {progressResult}
              </div>
              <div style={{padding: "15px", border: "2px solid grey", borderRadius: "6px"}}>
                <h2>api/state</h2>
                Player Id: <input type="text" name="playerId" size="50" value={playerId} onChange={handlePlayerIdChange} />
                <br/><br/>
                <button onClick={state}>Get Quest State</button>
                <br/>
                {stateResult}
              </div>
            </div>
            
        </div>
    );
}

export default App;