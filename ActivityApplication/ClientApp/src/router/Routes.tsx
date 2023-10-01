import {createBrowserRouter, RouteObject} from "react-router-dom";
import App from "../App";
import React from "react";
import HomePage from "../features/home/HomePage";
import ActivityDashboard from "../features/activity/ActivityDashboard";
import ActivityForm from "../features/activity/form/ActivityForm";

export const routes: RouteObject[] = [
	  {
			path: "/",
			element: <App />,
			children: [
				  {path: "", element: <HomePage />},
				  {path: "activities", element: <ActivityDashboard />},
				  {path: "createActivity", element: <ActivityForm />},
			]
	  }
]

export const router = createBrowserRouter(routes)