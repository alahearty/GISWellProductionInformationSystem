<template>
	<section>
		<Navbar/>
		<router-view/>
	</section>
</template>

<script lang='ts'>
import {Vue, Component} from 'vue-property-decorator';

import Navbar from '@/components/navbar/navbar.vue';

@Component({
	components: {
		Navbar,
	}
})
export default class Explorer extends Vue {
}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

.content {
  padding-top: 2.5em;
  transition: 0.5s;
}

@media (max-width: 850px) {
  .content {
	padding-left: 0px;
  }
}

.top-ribbon {
  margin-top: 40px;
}

.panes {
	display: flex;
	position: relative;
	height: 100vh;

	.center-pane {
		margin-left: 20%;
		width: 80%;
		height: 100vh;
		overflow-y: auto;
		display: flex;
	}

	.left-pane, .right-pane {
		position: absolute;
		top: 0;
		bottom: 0;
		background-color: $color-secondary-light;
	}

	.left-pane {
		overflow: hidden;
		z-index: 20;
		width: 20%;

	.resizer {
		position: absolute;
		z-index: 5000;
		top: 0;
		right: 0;
		bottom: 0;
		width: 10px;
		cursor: ew-resize;
	}
}

	.right-pane {
		right: 0;
	}
}
</style>