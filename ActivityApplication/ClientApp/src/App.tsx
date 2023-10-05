import React from 'react';
import {Container} from "semantic-ui-react";
import NavBar from "./components/navbar";
import {observer} from "mobx-react-lite";
import {Outlet, useLocation} from "react-router-dom";
import HomePage from "./features/home/HomePage";

const App: React.FC = () => {
      const location = useLocation();

  return (
        <>
              {location.pathname === "/" ? <HomePage /> : (
                    <>
                          <NavBar />
                          <Container style={{marginTop: "7rem"}}>
                                {/*? Outlet is a special component that will render the child route of the parent route - Same as Angular*/}
                                <Outlet/>
                          </Container>
                    </>
              )}
        </>
  );
};

export default observer(App);
