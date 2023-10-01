import React from 'react';
import {Container} from "semantic-ui-react";
import NavBar from "./components/navbar";
import {observer} from "mobx-react-lite";
import {Outlet} from "react-router-dom";

const App: React.FC = () => {

  return (
        <>
          <NavBar />
              <Container style={{marginTop: "7rem"}}>
                    {/*? Outlet is a special component that will render the child route of the parent route - Same as Angular*/}
                   <Outlet/>
              </Container>
          
        </>
  );
};

export default observer(App);
