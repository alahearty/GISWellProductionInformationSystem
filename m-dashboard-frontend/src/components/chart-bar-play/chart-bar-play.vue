<template>
	<section>
	<svg id="chart" style="width: 80vw;height: 80vh;"></svg>
	<!-- <div id="chartDiv" style="width:100%; height: 650px"></div> -->
	</section>
</template>

<script lang='ts'>
// @ts-nocheck

import {Vue, Component, Watch} from 'vue-property-decorator';
import * as d3 from 'd3';
import { fetchProductionDataByDate, getProductionDates } from '@/modules/Explorer/store';
import TotalProduction from '@/types/TotalProduction';
type ChartData = {
	date: string;
	name: string;
	category: string;
	value: string;
}
@Component
export default class ChartBarPlay extends Vue {
	private data: any = '';
	// mounted() {
	//   console.log(d3)
	//   fetch('http://localhost:8000', {
	//     method: 'GET'
	//   })
	//   .then(response => response.json())
	//   .then(body => { 
	//     // @ts-ignore
	//     this.data = d3.group(body, d => d.name)
	//     console.log(body)
	//     // @ts-ignore
	//     // const chartData = d3.group(body, d => d.name)
	//   })
	// }
	// get names() {
	//   // @ts-ignore
	//   // return new Set(Array.from(this.data.keys()))
	//   return new Set(this.data.map(d => d.name))
	// }
	// get dateValues() {
	//   // @ts-ignore
	//   return Array.from(d3.rollup(this.data, ([d]) => d.value, d => +d.date, d => d.name))
	//     .map(([date, data]) => [new Date(date), data])
	//     // @ts-ignore
	//     .sort(([a], [b]) => d3.ascending(a, b))
	// }
	
	private assetProductions: (TotalProduction & {prodDate: string})[] = [];

	get productionDates() {
		return getProductionDates(this.$store);
	}

	@Watch('productionDates')
	onProductionDates() {
		for (let i = 0; i < 2; i++) {
			const data = 
			fetchProductionDataByDate(this.$store, this.productionDates[i])
				.then((production: TotalProduction) => {
					this.assetProductions.push({
						prodDate: this.productionDates[i],
						...production
					})

					if (this.assetProductions.length == 2) {
						this.plotChart()
						// const dateList = Array.from(this.processedData.keys())
						// console.log('--', dateList)
						// let presentData = {}
						// for (const date of dateList) {
						// 	console.log('x', this.processedData.get(date))
						// 	presentData = this.processEachDateData(this.processedData.get(date))
						// 	console.log(prese)
						// }
					}
				})
		}
	}

	get processedData() {
		return d3.group(this.assetProductions, d => d.prodDate)
	}

	created() {
		fetch('https://api.covid19india.org/states_daily.json')
			.then(res => res.json())
			.then(data=> {
				console.log(data)

				const totalCovidDataState = data.states_daily 
				const groupedData = this.processData(totalCovidDataState);
				// console.log(groupedData); 
				// this.plotChart(groupedData)
			})
	}

	private colors: string[] = ['#eb5234', '#eba234', '#eba234', '#59c91c', '#1cc9bb', '#1d45ab', '#d417d4'];
	private randomColor() {
		return this.colors[Math.floor(Math.random() * this.colors.length)]
	}
	// private processData(data) { 
	// 	return d3.group(data, d => d.date, e => e.status);
	// }

	private processData(data) { 
		return d3.group(data, d => d.date, e => e.status);
	}
	
	private processEachDateData(data) {
		//remove status and date
		delete data.date
		delete data.status
		delete data.tt // tt is total 
		return Object.keys(data)
				.map(key => ({key, value: parseInt(data[key])}))
				// .sort((a,b) => b.value-a.value) 
	}

	async private plotChart() {
		console.log('now plotting')
		const data = this.processedData;
		const svg = d3.select("#chart")
		const width = svg.node().clientWidth;
		const height = svg.node().clientHeight;
		const ticker = 500;
		const dateList = Array.from(data.keys())
		const fontSize = 16;
		const rectProperties = {height: 20, padding: 10}
		const container = svg.append("g")
								.classed("container", true)
								.style("transform", "translateY(25px)")
		const widthScale = d3.scaleLinear()
		const axisTop = svg
						.append('g')
						.classed('axis', true)
						.style("transform", "translate(10px, 20px)")
						.call(d3.axisTop(widthScale))
		const update = (date) =>  {
			console.log('--', data.get(date))
			const presentData = this.processEachDateData(data.get(date)[0]['TotalOilRate'])
			console.log('--', presentData)
			// const presentData = this.processEachDateData(data.get(date).get("Confirmed")[0])
			widthScale.domain([0, d3.max(Object.values(presentData), d => d.value)])
					.range([0, width - fontSize - 50])
			axisTop                
				.transition()
				.duration(ticker / 1.2)
				.ease(d3.easeLinear)
				.call(d3.axisTop(widthScale))
			const sortedRange = [...presentData].sort((a,b) => b.value - a.value)
			container
				.selectAll("text")
				.data(presentData)
				.enter()
				.append("text")
			container
				.selectAll("text")
				.text(d => d.key + " "+ d.value)
				.transition()
				.delay(500)
				.attr("x", d => widthScale(d.value) + fontSize)
				.attr("y", (d,i) => sortedRange.findIndex(e => e.key === d.key) * (rectProperties.height + rectProperties.padding) + fontSize) 
			container
				.selectAll("rect")
				.data(presentData)
				.enter()
				.append("rect")
			container
				.selectAll("rect")
				.attr("x", 10)
				.attr("fill", (d, i) => this.randomColor())
				.transition()
				.delay(500)
				.attr("y", (d,i) => sortedRange.findIndex(e => e.key === d.key) * (rectProperties.height + rectProperties.padding))
				.attr("width", d => d.value <= 0? 0 : widthScale(d.value))
				.attr("height", 20)
		}
		console.log(dateList)
		for (const date of dateList) {
			update(date)
			await new Promise(done => setTimeout(() => done(), ticker));
		} 
	}
	
	// async private plotChart(data) {
	// 	const svg = d3.select("#chart")
	// 	const width = svg.node().clientWidth;
	// 	const height = svg.node().clientHeight;
	// 	const ticker = 500;
	// 	const dateList = Array.from(data.keys())
	// 	const fontSize = 16;
	// 	const rectProperties = {height: 20, padding: 10}
	// 	const container = svg.append("g")
	// 							.classed("container", true)
	// 							.style("transform", "translateY(25px)")
	// 	const widthScale = d3.scaleLinear()
	// 	const axisTop = svg
	// 					.append('g')
	// 					.classed('axis', true)
	// 					.style("transform", "translate(10px, 20px)")
	// 					.call(d3.axisTop(widthScale))
	// 	const update = (date) =>  {
	// 		const presentData = this.processEachDateData(data.get(date).get("Confirmed")[0])
	// 		widthScale.domain([0, d3.max(Object.values(presentData), d => d.value)])
	// 				.range([0, width - fontSize - 50])
	// 		axisTop                
	// 			.transition()
	// 			.duration(ticker / 1.2)
	// 			.ease(d3.easeLinear)
	// 			.call(d3.axisTop(widthScale))
	// 		const sortedRange = [...presentData].sort((a,b) => b.value - a.value)
	// 		container
	// 			.selectAll("text")
	// 			.data(presentData)
	// 			.enter()
	// 			.append("text")
	// 		container
	// 			.selectAll("text")
	// 			.text(d => d.key + " "+ d.value)
	// 			.transition()
	// 			.delay(500)
	// 			.attr("x", d => widthScale(d.value) + fontSize)
	// 			.attr("y", (d,i) => sortedRange.findIndex(e => e.key === d.key) * (rectProperties.height + rectProperties.padding) + fontSize) 
	// 		container
	// 			.selectAll("rect")
	// 			.data(presentData)
	// 			.enter()
	// 			.append("rect")
	// 		container
	// 			.selectAll("rect")
	// 			.attr("x", 10)
	// 			.attr("fill", (d, i) => this.randomColor())
	// 			.transition()
	// 			.delay(500)
	// 			.attr("y", (d,i) => sortedRange.findIndex(e => e.key === d.key) * (rectProperties.height + rectProperties.padding))
	// 			.attr("width", d => d.value <= 0? 0 : widthScale(d.value))
	// 			.attr("height", 20)
	// 	}
	// 	console.log(dateList)
	// 	for (const date of dateList) {
	// 		update(date)
	// 		await new Promise(done => setTimeout(() => done(), ticker));
	// 	} 
	// }
}
</script>