import React, {useEffect, useState} from "react";
import {Button, Header, Segment} from "semantic-ui-react";
import {useStore} from "../../../stores/store";
import {observer} from "mobx-react-lite";
import {Link, useNavigate, useParams} from "react-router-dom";
import {ActivityFormValues} from "../../../types/activity";
import LoadingComponent from "../../../components/LoadingComponent";
import {Formik, Form} from "formik";
import * as Yup from "yup";
import MyTextInput from "../../../components/Form/MyTextInput";
import MyTextArea from "../../../components/Form/MyTextArea";
import MySelectInput from "../../../components/Form/MySelectInput";
import {categoryOptions} from "../../../types/options/categoryOptions";
import MyDateInput from "../../../components/Form/MyDateInput";

export default observer(function ActivityForm()
{
    const {activityStore} = useStore();
    const {id} = useParams();
    const navigate = useNavigate();
    
    const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues())
    
    const {
        createActivity,
        updateActivity,
        loadActivity,
        loadingInitial
    } = activityStore
    
    // Takes property from our form
    const validationSchema = Yup.object({
        title: Yup.string().required("The activity title is required!"),
        description: Yup.string().required("The activity description is required!"),
        category: Yup.string().required(),
        date: Yup.string().required("The date is required!"),
        venue: Yup.string().required(),
        city: Yup.string().required(),
    })
    
    useEffect(() => {
        if(id) loadActivity(id)
              // Since we know that in this case DEFINITELY we will have an activity, we will not consider TypeScript's error
              .then(activity => setActivity(new ActivityFormValues(activity)))
    }, [id, loadActivity]);

    
    function handleFormSubmit(activity: ActivityFormValues)
    {
        // If we do have an id, then we are updating the activity, otherwise, we are creating a new one
        if (!activity.id)
            createActivity(activity)
                  .then(() => navigate(`/activities/${activity.id}`))
        else
            updateActivity(activity)
                  .then(() => navigate(`/activities/${activity.id}`))
    }

    /*function handleOnChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>)
    {
        const{name, value} = event.target
        setActivity({...activity, [name]: value})
    }*/
    if (loadingInitial) return <LoadingComponent content={"Loading the activity..."}/>
    
    return (
        <Segment clearing>
            <Header content={"Activity Details"} sub color={"teal"}/>
            {/*When we use a form for 2 different approaches, for instance create and edit, we have to set the Formik as enableReinitialize*/}
            <Formik 
                  validationSchema={validationSchema} 
                  enableReinitialize 
                  initialValues={activity} 
                  onSubmit={values => handleFormSubmit(values)}>
                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                      /*In order to get Semantic UI css, we specify the className for the Form, although we are using from Formik*/
                      <Form className={"ui form"} onSubmit={handleSubmit} autoComplete={"off"}>
                          <MyTextInput placeholder={"Title"} name={"title"} />
                          {/*! Form input elements*/}
                          <MyTextArea
                                placeholder={"Description"}
                                name={"description"} 
                                rows={3}/>
                          <MySelectInput
                                placeholder={"Category"}
                                name={"category"} 
                                option={categoryOptions}/>
                          <MyDateInput
                                placeholderText={"Date"}
                                name={"date"}
                                showTimeSelect
                                timeCaption={"time"}
                                dateFormat={"MMMM d, yyyy h:mm aa"}/>
                          
                          <Header content={"Location Details"} sub color={"teal"}/>
                          
                          <MyTextInput
                                placeholder={"City"}
                                name={"city"}/>
                          <MyTextInput
                                placeholder={"Venue"}
                                name={"venue"}/>

                          {/*! BUTTONS */}
                          {/* If the form is being submitted, or it is not dirty, or it is not valid, the Submit button will be disabled*/}
                          <Button
                                floated={"right"}
                                positive
                                type={"submit"}
                                content={"Submit"}
                                loading={isSubmitting}
                                onSubmit={handleSubmit}
                                disabled={isSubmitting || !dirty || !isValid}
                          />
                          <Button
                                floated={"right"}
                                type={"button"}
                                content={"Cancel"}
                                as={Link}
                                to={'/activities'}
                          />
                      </Form>
                )}
            </Formik>
        </Segment>
    )
})