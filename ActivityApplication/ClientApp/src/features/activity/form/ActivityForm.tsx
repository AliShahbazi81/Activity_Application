import React, {ChangeEvent, useState} from "react";
import {Button, Form, Segment} from "semantic-ui-react";
import {useStore} from "../../../stores/store";
import {observer} from "mobx-react-lite";

export default observer(function ActivityForm()
{
    const {activityStore} = useStore();
    const {
        selectedActivity,
        editMode, 
        createActivity,
        updateActivity,
        loading} = activityStore
    
    const initialState = selectedActivity ?? {
        id: "",
        title: "",
        description: "",
        category: "",
        date: "",
        city: "",
        venue: ""
    }

    const [activity, setActivity] = useState(initialState);
    
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
    return (
        <Segment clearing>
            {editMode &&
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
            </Form>}
        </Segment>
    )
})