import {Route, Routes} from "react-router";
import Landing from "./Pages/Landing.tsx";
import NavBar from "./Components/NavBar.tsx";
import Chatroom from "./Pages/Chatroom.tsx";
import {UserProvider} from "./UserProvider.tsx";

function App() {

  return (
      <>
          <UserProvider>
              <div className="flex flex-col h-screen">
                  <NavBar />

                  <div className="flex-1 overflow-hidden">
                      <Routes>
                          <Route path="/" Component={Landing} />
                          <Route path="/room/:roomSlug" Component={Chatroom} />
                      </Routes>
                  </div>
              </div>
          </UserProvider>
      </>
  )
}

export default App
