import React from "react";
import {Header, Menu} from "semantic-ui-react";
import {Calendar} from "react-calendar";

export default function ActivityFilters()
{
	  return(
			<>
				  <Menu vertical size={'large'} style={{width: '100', marginTop: "25px"}}>
						<Header
							  attached
							  icon={'filter'}
							  color={"teal"}
							  content={"Filters"}
						/>
						<Menu.Item content={"All Activities"}/>
						<Menu.Item content={"I'm going"}/>
						<Menu.Item content={"I'm hosting"}/>
				  </Menu>
				  <Header />
				  <Calendar />
			</>
			
	  )
}