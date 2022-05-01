import Vue from 'vue';
import { DirectoryOptions } from '@/types/FileExplorer';

type State = {
    activeNode: DirectoryOptions | null;
}

const nodeState = Vue.observable<State>({
    activeNode: null,
})

export const changeActiveNode = (node: DirectoryOptions) => { 
    nodeState.activeNode = node;
}

export const updateActiveNode = (node: DirectoryOptions) => {
    if (nodeState.activeNode) {
        if (nodeState.activeNode.children) {
            nodeState.activeNode.children.push(node)
        } else {
            Vue.set(nodeState.activeNode, 'children', [node])
        }
    }
}

export const hideActiveNode = () => {
    if (nodeState.activeNode) {
        nodeState.activeNode.showOptions = false;
        nodeState.activeNode.editName = false;
    }
}

window.addEventListener('click', () => {
    if (nodeState.activeNode) {
        hideActiveNode();
    }
})

export default nodeState 