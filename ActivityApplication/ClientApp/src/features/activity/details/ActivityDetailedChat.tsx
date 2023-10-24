import {Field, FieldProps, Form, Formik} from 'formik';
import {observer} from 'mobx-react-lite'
import React, {useEffect} from 'react'
import {Link} from 'react-router-dom';
import {Comment, Header, Loader, Segment} from 'semantic-ui-react'
import {useStore} from "../../../stores/store";
import * as Yup from "yup";
import {formatDistanceToNow} from "date-fns";

interface Props {
	  activityId: string;
}

export default observer(function ActivityDetailedChat({activityId}: Props) {
	  //! When we recall it, we are going to make connection
	  const {commentStore} = useStore();

	  useEffect(() => {
			// The most important factor for using hub is the activityId, So, we have to make sure if we DO have the activityId in this case
			// Because if we do not have any id, making connection is a waste of resource and time
			if (activityId)
				  commentStore.createHubConnection(activityId);
			// We have to dispose the connection when the component is disposed
			return () => {
				  commentStore.clearComments();
			}
	  }, [activityId, commentStore]);

	  return (
			<>
				  <Segment
						textAlign='center'
						attached='top'
						inverted
						color='teal'
						style={{border: 'none'}}
				  >
						<Header>Chat about this event</Header>
				  </Segment>
				  <Segment attached clearing>
						<Formik
							  initialValues={{body: ''}}
							  onSubmit={(values, {resetForm}) => {
									commentStore.addComment(values).then(() => resetForm())
							  }}
							  validationSchema={Yup.object({
									body: Yup.string().required()
							  })}
						>
							  {({isSubmitting, isValid, handleSubmit}) => (
									<Form className={"ui form"}>
										  <Field name={"body"}>
												{(props: FieldProps) => (
													  <div style={{position: "relative"}}>
															<Loader active={isSubmitting}/>
															<textarea
																  placeholder={"Enter your comment (Enter to submit, SHIFT + enter for new line"}
																  rows={2}
																  {...props.field}
																  onKeyDown={e => {
																		if (e.key === "Enter" && e.shiftKey)
																			  return
																		if (e.key === "Enter" && !e.shiftKey) {
																			  e.preventDefault();
																			  isValid && handleSubmit();
																		}
																  }}
															/>
													  </div>
												)}
										  </Field>
									</Form>
							  )}
						</Formik>
						<Comment.Group>
							  {commentStore.comments.map((comment) => (
									<Comment key={comment.id}>
										  <Comment.Avatar src={comment.image || '/assets/user.png'}/>
										  <Comment.Content>
												<Comment.Author as={Link} to={`/profiles/${comment.username}`}>
													  {comment.displayName}
												</Comment.Author>
												<Comment.Metadata>
													  <div>{formatDistanceToNow(comment.createdAt) + " ago"}</div>
												</Comment.Metadata>
												<Comment.Text style={{whiteSpace: "pre-wrap"}}>{comment.body}</Comment.Text>
										  </Comment.Content>
									</Comment>
							  ))}
						</Comment.Group>
				  </Segment>
			</>
	  )
})