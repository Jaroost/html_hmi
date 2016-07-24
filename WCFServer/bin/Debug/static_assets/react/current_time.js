class Time extends React.Component{
	render(){
		const now=new Date();
		return (
			<div className="container">Current time: {now.toTimeString()}</div>
		);
	}
}

ReactDOM.render(<Time/>, $('#time_container')[0]);