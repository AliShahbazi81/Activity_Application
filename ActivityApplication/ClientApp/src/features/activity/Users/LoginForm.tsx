import {Form, Formik} from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import { Button } from "semantic-ui-react";
import MyTextInput from "../../../components/Form/MyTextInput";
import {useStore} from "../../../stores/store";

export default observer(function LoginForm()
{
	  const {userStore} = useStore();
	  return(
			<Formik 
				  initialValues={{email: "", password: ""}} 
				  onSubmit={values => userStore.login(values)}
			>
				  {/* Formik will automatically realize when it has to turn on and off the loading. Hence, we do not have to create isSubmitting functions*/}
				  {({handleSubmit, isSubmitting}) => (
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