import React from "react";
import {observer} from "mobx-react-lite";
import {useStore} from "../../stores/store";
import {Form, Formik} from "formik";
import * as Yup from "yup";
import {Button } from "semantic-ui-react";
import MyTextInput from "../../components/Form/MyTextInput";
import MyTextArea from "../../components/Form/MyTextArea";

interface Props {
	  setEditMode: (editMode: boolean) => void;
}
export default observer(function ProfileEditForm({setEditMode}: Props) {
	  const {profileStore: {profile, updateProfile}} = useStore();
	  return (
			<Formik
				  initialValues={{displayName: profile?.displayName, bio:
						profile?.bio}}
				  onSubmit={values => {
						updateProfile(values).then(() => {
							  setEditMode(false);
						})
				  }}
				  validationSchema={Yup.object({
						displayName: Yup.string().required()
				  })}
			>
				  {({isSubmitting, isValid, dirty}) => (
						<Form className='ui form'>
							  <MyTextInput placeholder='Display Name'
										   name='displayName' />
							  <MyTextArea rows={3} placeholder='Add your bio'
										  name='bio' />
							  <Button
									positive
									type='submit'
									loading={isSubmitting}
									content='Update profile'
									floated='right'
									disabled={!isValid || !dirty}
							  />
						</Form>
				  )}
			</Formik>
	  )
})