import {ErrorMessage, Form, Formik} from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import {Button, Label} from "semantic-ui-react";
import MyTextInput from "../../../components/Form/MyTextInput";
import {useStore} from "../../../stores/store";

export default observer(function LoginForm()
{
	  const {userStore} = useStore();
	  return(
			/* setErrors in Formik allows us to catch the errors inside the Formik tag*/
			<Formik 
				  initialValues={{email: "", password: "", error: null}} 
				  onSubmit={(values, {setErrors}) => 
						userStore.login(values)
							  .catch(error => setErrors({error: "Invalid email or password"}))}
			>
				  {/* Formik will automatically realize when it has to turn on and off the loading. Hence, we do not have to create isSubmitting functions*/}
				  {({handleSubmit, isSubmitting, errors}) => (
						<Form 
							  className={"ui form"} 
							  onSubmit={handleSubmit} 
							  autoComplete={"off"}>
							  <MyTextInput 
									placeholder={"Email"} 
									name={"email"} />
							  <MyTextInput 
									placeholder={"Password"} 
									name={"password"} 
									type={"password"}/>
							  <ErrorMessage 
									name={"error"} 
									render={() => 
										  <Label style={{marginBottom: 10}} basic color={"red"} content={errors.error} />
							  }/>
							  <Button
									positive
									fluid
									loading={isSubmitting}
									content={"Login"} 
									type={"submit"} 
									/>
						</Form>
				  )}
			</Formik>
	  )
})