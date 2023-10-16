import { useField} from "formik";
import React from "react";
import {Form, Label, Select} from "semantic-ui-react";

interface Props{
	  placeholder: string;
	  name: string;
	  option: any;
	  label?: string;
}

export default function MySelectInput(props: Props)
{
	  /* Helpers to manually set a value and manually set the status of touch */
	  const [field, meta,helpers] = useField(props.name);
	  return(
			/* (!!) makes an object boolean -> whether exists or not */
			<Form.Field error={meta.touched && !!meta.error}>
				  <label>{props.label}</label>
				  <Select
						clearable
						options={props.option} 
				  		value={field.value || null}
						onChange={(e, data) => helpers.setValue(data.value)}
						onBlur={() => helpers.setTouched(true)}
						placeholder={props.placeholder}
				  />
				  {meta.touched && meta.error ? (
						<Label basic color={"red"}>
							  {meta.error}
						</Label>
				  ): null}
			</Form.Field>
	  )
}