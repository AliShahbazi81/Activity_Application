import {ErrorMessage, Form, Formik} from "formik";
import { observer } from "mobx-react-lite";
import React from "react";
import {Button, Header, Label} from "semantic-ui-react";
import MyTextInput from "../../../components/Form/MyTextInput";
import {useStore} from "../../../stores/store";
import * as Yup from "yup"; 

export default observer(function RegisterForm()
{
	  const {userStore} = useStore();
	  return(
			/* setErrors in Formik allows us to catch the errors inside the Formik tag*/
			/* Client-side inputs validation will be checked using validationSchema in Formik */
			<Formik
				  initialValues={{displayName:"", username:"", email: "", password: "", error: null}}
				  onSubmit={(values, {setErrors}) =>
						userStore.login(values)
							  .catch(error => setErrors({error: "Invalid email or password"}))}
				  validationSchema={Yup.object({
						displayName: Yup.string().required(),
						username: Yup.string().required(),
						email: Yup.string().required(),
						password: Yup.string().required(),
				  })}
			>
				  {/* Formik will automatically realize when it has to turn on and off the loading. Hence, we do not have to create isSubmitting functions*/}
				  {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
						<Form
							  className={"ui form"}
							  onSubmit={handleSubmit}
							  autoComplete={"off"}>
							  <Header
									content={"Signup to Reactivities"}
									color={"teal"}
									textAlign={"center"}/>
							  <MyTextInput
									placeholder={"Display Name"}
									name={"displayName"} />
							  <MyTextInput
									placeholder={"Username"}
									name={"username"} />
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
									disabled={!isValid || !dirty || isSubmitting}
									loading={isSubmitting}
									content={"Register"}
									type={"submit"}
							  />
						</Form>
				  )}
			</Formik>
	  )
})