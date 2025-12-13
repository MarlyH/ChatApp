import {Route, Routes} from "react-router";
import Landing from "./Pages/Landing.tsx";
import NavBar from "./Components/NavBar.tsx";
import Chatroom from "./Pages/Chatroom.tsx";

function App() {

  return (
      <>
          <NavBar />
          <div className="flex flex-col items-center justify-center h-screen">
              <Routes>
                  <Route path="/" Component={Landing}  />
                  <Route path="/room/:roomSlug" Component={Chatroom} />
              </Routes>
          </div>
      </>
  )
}

export default App
