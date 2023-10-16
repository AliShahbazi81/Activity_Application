import React, {StrictMode} from "react";
import "semantic-ui-css/semantic.min.css";
import "react-calendar/dist/Calendar.css"
import "./styles/styles.css";
import "react-toastify/dist/ReactToastify.min.css"
import "react-datepicker/dist/react-datepicker.css"
import {createRoot} from "react-dom/client";
import {store, StoreContext} from "./stores/store";
import {RouterProvider} from "react-router-dom";
import {router} from "./router/Routes";

const rootElement = document.getElementById("root");
if (!rootElement) throw new Error('Failed to find the root element')
const root = createRoot(rootElement);

root.render(
	  <StrictMode>
			<StoreContext.Provider value={store}>
				<RouterProvider router={router} />
			</StoreContext.Provider>
	  </StrictMode>
);