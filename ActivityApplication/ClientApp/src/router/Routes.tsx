import {createBrowserRouter, RouteObject} from "react-router-dom";
import App from "../App";
import React from "react";
import ActivityDashboard from "../features/activity/ActivityDashboard";
import ActivityForm from "../features/activity/form/ActivityForm";
import ActivityDetails from "../features/activity/ActivityDetails";

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
			]
	  }
]

export const router = createBrowserRouter(routes)