import { useField} from "formik";
import React from "react";
import {Form, Label} from "semantic-ui-react";
import DatePicker, {ReactDatePickerProps} from "react-datepicker"; 

/*When specify as Partial, we make all the attributes optional even if it is not*/
export default function MyDateInput(props: Partial <ReactDatePickerProps>)
{
	  const [field, meta, helpers] = useField(props.name!);
	  return(
			/* (!!) makes an object boolean -> whether exists or not */
			<Form.Field error={meta.touched && !!meta.error}>
				  <DatePicker 
						{...field} 
						{...props} 
				  		selected={(field.value && new Date(field.value)) || null}
						onChange={value => helpers.setValue(value)}
				  />
				  {meta.touched && meta.error ? (
						<Label basic color={"red"}>
							  {meta.error}
						</Label>
				  ): null}
			</Form.Field>
	  )
}