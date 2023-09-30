import React, {StrictMode} from "react";
import App from "./App";
import "semantic-ui-css/semantic.min.css";
import "./styles/styles.css";

import {createRoot} from "react-dom/client";

const rootElement = document.getElementById("root");
if (!rootElement) throw new Error('Failed to find the root element')
const root = createRoot(rootElement);

root.render(
	  <StrictMode>
			<App />
	  </StrictMode>
);