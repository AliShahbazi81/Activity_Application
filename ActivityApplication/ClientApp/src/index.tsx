import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import "semantic-ui-css/semantic.min.css"
import "./styles/styles.css"



const rootElement = document.getElementById("root");

if (rootElement) {
      ReactDOM.render(
            <BrowserRouter>
                  <App />
            </BrowserRouter>,
            rootElement
      );
} else {
      console.error("Root element not found");
}
