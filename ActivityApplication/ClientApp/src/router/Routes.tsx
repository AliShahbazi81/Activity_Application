import {createBrowserRouter, Navigate, RouteObject} from "react-router-dom";
import App from "../App";
import React from "react";
import ActivityDashboard from "../features/activity/dashboard/ActivityDashboard";
import ActivityForm from "../features/activity/form/ActivityForm";
import ActivityDetails from "../features/activity/details/ActivityDetails";
import TestErrors from "../features/errors/TestError";
import NotFound from "../features/errors/NotFound";
import ServerError from "../features/errors/ServerError";
import LoginForm from "../features/activity/Users/LoginForm";

export const routes: RouteObject[] = [
	  {
			path: "/",
			element: <App />,
			children: [
				  {path: "activities", element: <ActivityDashboard />},
				  {path: "activities/:id", element: <ActivityDetails />},
				  //! Since both create and manage are using the same component, we HAVE TO set key property to them, otherwise, react cannot
				  //! understand the difference when we route between these 2 components
				  {path: "createActivity", element: <ActivityForm key={"create"}/>},
				  /* Editing an activity will be done in the ActivityForm as well*/
				  {path: "manage/:id", element: <ActivityForm key={"manage"}/>},
				  {path: "login", element: <LoginForm />},
				  {path: "errors", element: <TestErrors />},
				  {path: "not-found", element: <NotFound />},
				  {path: "server-error", element: <ServerError />},
				  
				  // The route down below indicates that when user heads to whatever pages that do not exist in the application
				  {path: "*", element: <Navigate replace to={"/not-found"}/>},
			]
	  }
]

export const router = createBrowserRouter(routes)