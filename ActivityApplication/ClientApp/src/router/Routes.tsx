import {createBrowserRouter, RouteObject} from "react-router-dom";
import App from "../App";
import React from "react";
import HomePage from "../features/home/HomePage";
import ActivityDashboard from "../features/activity/ActivityDashboard";
import ActivityForm from "../features/activity/form/ActivityForm";
import ActivityDetails from "../features/activity/ActivityDetails";

export const routes: RouteObject[] = [
	  {
			path: "/",
			element: <App />,
			children: [
				  {path: "", element: <HomePage />},
				  {path: "activities", element: <ActivityDashboard />},
				  {path: "activities/:id", element: <ActivityDetails />},
				  {path: "createActivity", element: <ActivityForm />},
				  /* Editing an activity will be done in the ActivityForm as well*/
				  {path: "manage/:id", element: <ActivityForm />},
			]
	  }
]

export const router = createBrowserRouter(routes)