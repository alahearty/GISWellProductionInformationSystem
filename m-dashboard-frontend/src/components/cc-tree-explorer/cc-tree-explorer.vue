<template>
	<section>
			<ul v-for="(node, nodeIndex) in directoryNodes" :key="nodeIndex" class="pointer" 
				draggable
				@dragover.stop.prevent
				@dragenter.stop.prevent
				@dragend.stop="removeNode($parent.children, path, node, nodeIndex)"
				@drop.stop="onDrop($event, node)"
				@dragstart.stop="onDragStart($event, node)"
				@dblclick.stop="startEditing(node)"
				@click.left.stop="onRightClickFunc($event, node)"
				@click.right.prevent.stop="showNodeOptions($event, node)">
					<li class="d-flex align-items-center position-relative">
						<img 
							@click.left.stop="node.show = !node.show"
							v-if="node.type === 'folder'"
							class="folder-icon"
							src="@/assets/folder.svg" 
							alt="folder-icon">
						<img 
							v-if="node.type === 'file'"
							class="file-icon"
							src="@/assets/file.svg" 
							alt="file-icon">

							<input type="text" v-model="node.name" v-if="node.editName" 
								@keyup.enter="stopEditing(node)" 
								:style="{width: `${node.name.length || 7}ch`}">
							<span class="node-name" v-else>{{ node.name }}</span>

							<ul class="options" v-if="node.showOptions" :style="optionsPositionStyle">
								<template v-if="activeNode.type === 'folder'">
									<li @click="newNode('folder')">New Folder</li>
									<li @click="newNode('file')" class="border-bottom">New Well</li>
								</template>
								<li>View in new tab</li>
								<li>Well status</li>
								<li>Intervention Records</li>
								<li>Properties</li>
								<li>Anulus Data</li>
								<li>Generate Wellbook</li>
							</ul>
					</li>
					<ul v-if="node.children && node.children.length > 0" 
						v-show="node.show">
						<cc-tree-explorer 
							:directoryNodes="node.children" 
							:path="nodeIndex"
							:onRightClickFunc="onRightClickFunc"/>
					</ul>
			</ul>
	</section>
</template>

<script lang='ts'>
import {Vue, Component, Prop} from 'vue-property-decorator';
import nodeStore, { changeActiveNode, updateActiveNode, hideActiveNode } from './node.observable';

import Directory, { DirectoryOptions } from '@/types/FileExplorer';

type EventCallback<T = Event> = (event: T, node: Directory) => void;

@Component
export default class CcTreeExplorer extends Vue {
	@Prop({ default: () => []}) directoryNodes!: DirectoryOptions[];
	@Prop({ default: () => { return null }}) onRightClickFunc!: EventCallback;
	@Prop({ default: () => false }) allowDrag!: boolean;
	
	@Prop({ default: () => ''}) path!: string;

	get activeNode() {
		return nodeStore.activeNode;
	}

	private optionsPositionStyle: {[cssProperty: string]: string} = {};

	private showNodeOptions(event: MouseEvent, node: DirectoryOptions) {
		if (this.activeNode) {
			hideActiveNode()
		}
		const mousePosition: {[axis: string]: number} = {
			x: event.clientX,
			y: event.clientY 
		}

		this.optionsPositionStyle = {
			top: `${mousePosition.y}px`,
			left: `${mousePosition.x }px`
		}
		this.$set(node, 'showOptions', true);
		changeActiveNode(node)
	}

	private newNode(nodeType: 'file' | 'folder') {
		const newWell: DirectoryOptions = {
			name: '',
			type: nodeType,
			editName: true,
			showOptions: false,
			show: true,
		}

		if (nodeType === 'file') {
			newWell.data = {
				wellType: '',
				lift: '',
				completionType: '',
				wellConfiguration: '',
				spudDate: ''
			}
		}

		updateActiveNode(newWell)
		hideActiveNode()
	}

	private startEditing(node: DirectoryOptions) {
		this.$set(node, 'editName', true)
		changeActiveNode(node)
	}

	private stopEditing(node: Directory) {
		node.name = node.name || 'Untitled';
		this.$delete(node, 'editName')
	}

	private removeNode(parent: Directory[], parentNode: number, node: Directory, nodeIndex: number) {
		if (this.allowDrag) {
			// if parent is undefined it's a root node
			if (parent === undefined) {
				const node = this.directoryNodes[nodeIndex]
				if (node.type === 'file') {
					this.directoryNodes.splice(nodeIndex, 1)
				}
			} else {
				if (parent[parentNode].children) {
					parent[parentNode].children?.splice(nodeIndex, 1);
				}
			}
		}
	}

	
	private onDragStart(ev: DragEvent, dragContent: Directory) {
		if (this.allowDrag) {
			if (ev.dataTransfer != null) {
				ev.dataTransfer.dropEffect = 'copy';
				ev.dataTransfer.effectAllowed = 'move';
				ev.dataTransfer.setData('text/plain', JSON.stringify(dragContent));
			}
		}
	}

	private onDrop(ev: DragEvent, dropZoneContent: Directory) {
		if (dropZoneContent.type === 'folder') {
			if (ev.dataTransfer) {
				const dragContent = ev.dataTransfer.getData("text/plain");
				const dragContentData = JSON.parse(dragContent) as Directory;
				if (dragContent !== JSON.stringify(dropZoneContent)) {
					if (dropZoneContent.children) {
						dropZoneContent.children.push(dragContentData)
					} else {
						this.$set(dropZoneContent, 'children', dragContentData)
					}
				}
			}
		}
	}

}
</script>

<style lang="scss" scoped>
@import "@/scss/color.scss";

input {
	background-color: transparent;
	border: 1px solid gray;
	padding: 0;

	&:focus {
		border: none;
		outline: none;
	}
}

ul {
	list-style-type: none;
	padding-left: 10px;
	margin-bottom: 0;
	white-space: nowrap;

	li {
		padding: 3px 8px 3px 0;
	}
}

.node-name:hover {
	font-weight: bold;
}

.options {
	background-color: #fff;
	border-radius: 2px;
	padding-left: 0;
	position: fixed;
	z-index: 400;
	box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.04), 0px 0px 2px rgba(0, 0, 0, 0.06), 0px 0px 1px rgba(0, 0, 0, 0.04);

	li {
		padding: 5px 10px;

		&:hover {
			background-color: $color-secondary-light;
		}
	}
}

.folder-icon {
	height: 15px;
	margin-right: 5px;
}

.file-icon {
	height: 17px;
	margin-right: 11px;
}
</style>