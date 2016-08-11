class Machine extends React.Component{
	constructor(){
		super();
		
		this.state={
			machine:undefined
		}
	}
	
	componentDidMount(){
		const that=this;
		$.connection.hub.url = "https://192.168.178.21:8080/signalr";
		this.chat=$.connection.myHub;
		//this.chat = $.connection.myHub;
		this.chat.client.machine=function(machine){
			that.setState({machine});
		}
		$.connection.hub.start();
	}
	
	render(){
		if(this.state.machine!==undefined){
			const stations=this._getStations();
			return(
				<div className="container-fluid">
					<h1>Machine Component!</h1>
					<h3>{this.state.machine.Stations.length} stations</h3>
					<div>
						{stations}
					</div>					
				</div>		
			);
		}else{
			return (
				<div className="container-fluid">
					<h1>Machine Component!</h1>
				</div>		
			);
		}
		
	}
	
	_getStations(){
		return this.state.machine.Stations.map((station)=>{
			switch(station.Type){
				case "Unit":
					return (<UnitStation {...station} key={station.Num} />);
				case "Loader":
					return (<LoaderStation {...station} key={station.Num} />);
				case "Unloader":
					return (<UnloaderStation {...station} key={station.Num} />);
				default:
					return (<UnitStation {...station} key={station.Num} />);
			}
			//const StationType=`${station.Type}Station`;
			//return (<StationType {...station} key={station.Num} />);
		});
	}
}

class UnitStation extends React.Component{
	render(){
		const axis=this._getAxis();
		return (
			<div className="col-xs-2">
				<div className="well">
					Unit N° {this.props.Num}<br/>
					Axis:
					<ul>
					{axis}
					</ul>
				</div>
			</div>
		);
	}
	
	_getAxis(){
		let num=this.props.Num;
		return this.props.Axis.map((axe)=>{
			
			return (<Axe {...axe} key={axe.Name+num}/>)
		})
	}
	
}

class LoaderStation extends React.Component{
	render(){
		const robots=this._getRobots();
		return (
			<div className="col-xs-2">
				<div className="well">
					Loader N° {this.props.Num}<br/>
					Robots:
					<ul>
						{robots}
					</ul>
				</div>	
			</div>
		);
	}
	
	_getRobots(){
		let num=this.props.Num;
		return this.props.Robots.map((robot)=>{
			return (<Robot {...robot} key={robot.Key}/>);
		})
	}
}

class UnloaderStation extends React.Component{
	render(){
		const robots=this._getRobots();
		return (
		<div className="col-xs-2">
			<div className="well">
				Unloader N° {this.props.Num}<br/>
				Robots:
			</div>	
		</div>
		);
	}
	
	_getRobots(){
		let num=this.props.Num;
		return this.props.Robots.map((robot)=>{
			return (<Robot {...robot} key={robot.Key}/>);
		})
	}
}

class Station extends React.Component{
	render(){
		return(
			<div className="well">
				Station N° {this.props.Num}<br/>
				Axis: {this._getAxisLength(this.props.Axis)}
				
			</div>
		);
	}
	
	_getAxisLength(axis){
		if(axis!==undefined){
			return axis.length;
		}else{
			return 0;
		}
	}
}

class Axe extends React.Component{
	render(){
		if (this.props.Position>0){
			return (
				<li>
					<p>Axe: {this.props.Name}</p>
					<p>Minimum: {this.props.Minimum}</p>
					<p>Maximum: {this.props.Maximum}</p>
					<p className="good">Position: {this.props.Position}</p>
				</li>
			);
		}else{
			return (
				<li>
					<p>Axe: {this.props.Name}</p>
					<p>Minimum: {this.props.Minimum}</p>
					<p>Maximum: {this.props.Maximum}</p>
					<p className="bad">Position: {this.props.Position}</p>
				</li>
			);
		}
		
	}
}

class Robot extends React.Component{
	render(){
		return (
			<li><div>Robot: {this.props.Name}<br/>Serial: {this.props.SerialNumber}</div></li>
		);
	}
}

ReactDOM.render(<Machine />, $('#machine_container')[0]);