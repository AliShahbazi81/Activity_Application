import React from "react";
import {Dimmer, Loader} from "semantic-ui-react";

interface Props{
	  // inverted: Means that we are going to darken the background or give it a lighter background
	  inverted?: boolean
	  // Getting loading text
	  content?: string
}

export default function LoadingComponent({inverted = true, content = "Loading..."}: Props) {
	  return (
			<Dimmer active={true} inverted={inverted}>
				  <Loader content={content}/>
			</Dimmer>
	  )
}