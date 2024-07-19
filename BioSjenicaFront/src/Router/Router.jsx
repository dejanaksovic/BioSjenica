import { createBrowserRouter, createRoutesFromElements, Route, Routes } from "react-router-dom";
import Home from "../Pages/Home";
import Users from "../Pages/User/All"

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route>
      <Route index element = { <Home/> } />
      <Route path="/users" element = { <Users/> }/>
    </Route>
  )
)

export { router };