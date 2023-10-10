import React, {useEffect} from 'react';
import {Container} from "semantic-ui-react";
import NavBar from "./components/navbar";
import {observer} from "mobx-react-lite";
import {Outlet, useLocation} from "react-router-dom";
import HomePage from "./features/home/HomePage";
import {ToastContainer} from "react-toastify";
import {useStore} from "./stores/store";
import LoadingComponent from './components/LoadingComponent';
import ModalContainer from './components/modals/ModalContainer';

const App: React.FC = () => {
	  const location = useLocation();
	  const {userStore, commonStore} = useStore();

	  // Either user found or not, the loading spinner has to be stopped after finding or not finding the user
	  useEffect(() => {
			if (commonStore.token)
				  userStore.getUser().finally(() => commonStore.setAppLoaded())
			else
				  commonStore.setAppLoaded()
	  }, [userStore, commonStore]);
	  
	  if (!commonStore.appLoaded) return <LoadingComponent content={"Loading App..."}/>

	  return (
			<>
				  <ModalContainer />
				  {/*In order to show the Toastify error all around our app, we have to implement it here*/}
				  <ToastContainer position={"bottom-right"} hideProgressBar theme={"colored"}/>
				  {location.pathname === "/" ? <HomePage/> : (
						<>
							  <NavBar/>
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
