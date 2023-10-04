import React, {ChangeEvent, useEffect, useState} from "react";
import {Button, Form, Segment} from "semantic-ui-react";
import {useStore} from "../../../stores/store";
import {observer} from "mobx-react-lite";
import {useParams} from "react-router-dom";
import {Activity} from "../../../types/activity";
import LoadingComponent from "../../../components/LoadingComponent";

export default observer(function ActivityForm()
{
    const {activityStore} = useStore();
    const {id} = useParams();
    const {
        selectedActivity,
        createActivity,
        updateActivity,
        loading,
        loadActivity,
        loadingInitial
    } = activityStore
    
    const [activity, setActivity] = useState<Activity>({
        id: "",
        title: "",
        description: "",
        category: "",
        date: "",
        city: "",
        venue: ""
    })

    useEffect(() => {
        if(id) loadActivity(id)
              // Since we know that in this case DEFINITELY we will have an activity, we will not consider TypeScript's error
              .then(activity => setActivity(activity!))
    }, [id, loadActivity]);

    
    function handleSubmit()
    {
        // If we do have an id, then we are updating the activity, otherwise, we are creating a new one
        activity.id ? updateActivity(activity) : createActivity(activity)
    }
    
    function handleOnChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>)
    {
        const{name, value} = event.target
        setActivity({...activity, [name]: value})
    }
    if (loadingInitial) return <LoadingComponent content={"Loading the activity..."}/>
    
    return (
        <Segment clearing>
            <Form onSubmit={handleSubmit} autoComplete={"off"}>
                {/*! Form input elements*/}
                <Form.Input 
                      placeholder={"Title"} 
                      value={activity.title} 
                      name={"title"} 
                      onChange={handleOnChange}/>
                <Form.TextArea 
                      placeholder={"Description"} 
                      value={activity.description} 
                      name={"description"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Category"} 
                      value={activity.category} 
                      name={"category"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Date"} 
                      type={"date"}
                      value={activity.date} 
                      name={"date"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"City"} 
                      value={activity.city} 
                      name={"city"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Venue"} 
                      value={activity.venue} 
                      name={"venue"} 
                      onChange={handleOnChange}/>
                
                {/*! BUTTONS */}
                <Button 
                      floated={"right"} 
                      positive 
                      type={"submit"} 
                      content={"Submit"} 
                      loading={loading}
                      onClick={handleSubmit}
                />
                <Button 
                      floated={"right"} 
                      type={"button"} 
                      content={"Cancel"}
                />
            </Form>
        </Segment>
    )
})