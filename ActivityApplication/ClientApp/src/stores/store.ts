import ActivityStore from "./activityStore";
import {createContext, useContext} from "react";
import CommonStore from "./CommonStore";

interface Store{
	  activityStore: ActivityStore
	  commonStore: CommonStore
}

export const store: Store = {
	  activityStore: new ActivityStore(),
	  commonStore: new CommonStore()
}

export const StoreContext = createContext(store)

// Create React hook
export function useStore()
{
	  return useContext(StoreContext)
}