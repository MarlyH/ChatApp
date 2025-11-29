import {Route, Routes} from "react-router";
import Landing from "./Pages/Landing.tsx";
import Register from "./Pages/Register.tsx";

function App() {

  return (
    <div className="flex flex-col items-center justify-center h-screen">
        <Routes>
            <Route path="/" Component={Landing}  />
            <Route path={"/register"} Component={Register} />
        </Routes>
    </div>
  )
}

export default App
