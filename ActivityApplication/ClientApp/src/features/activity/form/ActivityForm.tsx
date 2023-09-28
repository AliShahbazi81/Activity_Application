import React, {ChangeEvent, useState} from "react";
import {Button, Form, Segment} from "semantic-ui-react";
import {Activity} from "../../../types/activity";

interface Props{
    activity: Activity | undefined
    editMode: boolean
    closeForm: () => void
    createOrEditActivity: (activity: Activity) => void
}
export default function ActivityForm({activity: selectedActivity, editMode, closeForm, createOrEditActivity}: Props)
{

    const initialState = selectedActivity ?? [{
        id: "",
        title: "",
        description: "",
        category: "",
        date: "",
        city: "",
        venue: ""
    }]

    const [activity, setActivity] = useState(initialState);
    
    function handleSubmit()
    {
        createOrEditActivity(activity as Activity)
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
                      value={selectedActivity?.title} 
                      name={"title"} 
                      onChange={handleOnChange}/>
                <Form.TextArea 
                      placeholder={"Description"} 
                      value={selectedActivity?.description} 
                      name={"description"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Category"} 
                      value={selectedActivity?.category} 
                      name={"category"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Date"} 
                      value={selectedActivity?.date} 
                      name={"date"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"City"} 
                      value={selectedActivity?.city} 
                      name={"city"} 
                      onChange={handleOnChange}/>
                <Form.Input 
                      placeholder={"Venue"} 
                      value={selectedActivity?.venue} 
                      name={"venue"} 
                      onChange={handleOnChange}/>
                
                {/*! BUTTONS */}
                <Button 
                      floated={"right"} 
                      positive 
                      type={"submit"} 
                      content={"Submit"} 
                      onClick={handleSubmit}
                />
                <Button 
                      onClick={closeForm}
                      floated={"right"} 
                      type={"button"} 
                      content={"Cancel"}
                />
            </Form>}
        </Segment>
    )
}