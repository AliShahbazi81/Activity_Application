import {Profile} from "./profile";

export interface Activity {
	  id: string
	  title: string
	  date: Date | null;
	  description: string
	  category: string
	  city: string
	  venue: string
	  hostUsername: string
	  isCancelled: boolean
	  isHost: boolean
	  isGoing: boolean
	  host?: Profile
	  attendees: Profile[]
}

//! Since we do not return any after creating or editing an activity, hence the array of attendees for the activity is empty.
// Either we can return data from server side, or we can take care of it in our front side.
// In this project, we decided to take care of it using Front-side
export class ActivityFormValues {
	  id?: string = undefined;
	  title: string = "";
	  category: string = "";
	  description: string = "";
	  date: Date | null = null;
	  city: string = "";
	  venue: string = "";
	  hostUsername?: string; 
	  attendees?: Profile[]; 
	  
	  //! Remember: Just the properties we want to edit have to be presented in the Ctor.
	  // Otherwise, the API will try to update all the properties listed in the Constructor which may cause flaws
	  constructor(activity?: ActivityFormValues) 
	  {
			if (activity)
			{
				  this.id = activity.id;
				  this.title = activity.title;
				  this.category = activity.category;
				  this.description = activity.description;
				  this.date = activity.date;
				  this.city	= activity.city;
				  this.venue = activity.venue;
				  this.hostUsername = activity?.hostUsername;
				  this.attendees = activity?.attendees;
			}
	  }
}

// The purpose of creating this class is, get all the ActivityFormValues properties and set it to Activity Interface
export class Activity implements Activity{
	  constructor(init?: ActivityFormValues)
	  {
			// The code below automatically sets all the ActivityFormValues props to Activity interface
			//! If you are using Vite, you have to first rename the interface to IActivity, then you have to assign all the required properties one by one
			Object.assign(this, init)
	  }
}